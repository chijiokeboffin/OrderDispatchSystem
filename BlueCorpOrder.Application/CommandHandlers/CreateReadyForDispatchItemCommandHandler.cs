using BlueCorpOrder.Application.Commands;
using BlueCorpOrder.Domain.Entities;
using BlueCorpOrder.Domain.RepositoryInterfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueCorpOrder.Application.CommandHandlers
{
    public class CreateReadyForDispatchItemCommandHandler(IReadyForDispatchRepository readyForDispatchRepository) : IRequestHandler<CreateReadForDispatchItemCommand, int>
    {
        public Task<int> Handle(CreateReadForDispatchItemCommand request, CancellationToken cancellationToken = default)
        {
            var containers = request.readyForDispatch.Containers.Select(x => new Container { ContainerType = x.ContainerType,  LoadId = x.LoadId, 
                Items = x.Items.Select(y => new Item { ItemCode = y.ItemCode, CartonWeight = y.CartonWeight, Quantity = y.Quantity }).ToList()}).ToList();
          
            var readyForDispatch = new ReadyForDispatch
            {
                ControlNumber = request.readyForDispatch.ControlNumber,
                SalesOrder = request.readyForDispatch.SalesOrder,
                DeliveryAddress = new DeliveryAddress
                {
                    Street = request.readyForDispatch.DeliveryAddress.Street,
                    City = request.readyForDispatch.DeliveryAddress.City,
                    State = request.readyForDispatch.DeliveryAddress.Street,
                    PostalCode = request.readyForDispatch.DeliveryAddress.PostalCode,
                    Country = request.readyForDispatch.DeliveryAddress.Country
                },
                Containers = containers
                
            };
            return readyForDispatchRepository.CreateAsync(readyForDispatch);
        }
    }
}
