using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using BlueCorpOrder.Application.Services.Abstractions;
using System.Net;
using BlueCorpOrder.Application.Dtos;
using BlueCorpOrder.Infrastructure.ThreePLServiceIntegration;
using BlueCorpOrder.Application.AppInsightConfiguration;
using MediatR;
using BlueCorpOrder.Application.Query;

namespace BlueCorp.Function
{
    public class DispatchFunction
    {
        private readonly IDispatchCSVService _dispatchCSVService;
        private readonly IThreePLService _threePLService;
        private readonly IInsightsTracker _insightsTracker;
        private readonly IMediator _mediator;
        public DispatchFunction(IDispatchCSVService dispatchCSVService, IThreePLService threePLService,
            IInsightsTracker insightsTracker, IMediator mediator)
        {
            _dispatchCSVService = dispatchCSVService ?? throw new ArgumentNullException(nameof(dispatchCSVService));
            _threePLService = threePLService ?? throw new ArgumentNullException(nameof(threePLService));
            _insightsTracker = insightsTracker ?? throw new ArgumentNullException(nameof(insightsTracker));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        [FunctionName("ReadyForDispatch")]
        public async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            _insightsTracker.TrackEventToInsights("ReadyForDispatch",  "Dispatch called ..", "Request");

            // Read request payload
            string requestJson = await new StreamReader(req.Body).ReadToEndAsync();

            _insightsTracker.TrackEventToInsights("ReadyForDispatch", requestJson, "Request");

            if (requestJson == null)
            {
                var errorResponse = new { message = "Invalid request payload.", success = false };

                _insightsTracker.TrackEventToInsights("ReadyForDispatch", requestJson, JsonConvert.SerializeObject(errorResponse));
                return new BadRequestObjectResult(errorResponse);
            }
               


            var dispatchData = JsonConvert.DeserializeObject<ReadyForDispatchDto>(requestJson);



            //Call a service to check if it is a new control number, if not, do not process, return
            var existingDispatch = await _mediator.Send(dispatchData);
            if (existingDispatch != null) {
                return new BadRequestObjectResult(new { message = "This has been processed", success = false });
            }





            // Transform JSON to CSV
            string csvData = _dispatchCSVService.ConvertObjectToCsv(dispatchData);

            
            bool isSuccess = await _threePLService.UploadFileToSftpAsync(csvData);
            if (!isSuccess)
            {
                var errorResponse = new { message = "Unable to upload the file to 3Pl server..", success = false };
                _insightsTracker.TrackEventToInsights("ReadyForDispatch", requestJson, JsonConvert.SerializeObject(errorResponse));
                return new BadRequestObjectResult(errorResponse);
            }

            var response = new { message = "Dispatch request processed successfully.", success = true };
            _insightsTracker.TrackEventToInsights("ReadyForDispatch", requestJson, JsonConvert.SerializeObject(response));

            return new OkObjectResult(response);
        }
    }
}
