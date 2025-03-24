using BlueCorpOrder.Application.Query;
using BlueCorpOrder.Domain.Entities;
using BlueCorpOrder.Domain.RepositoryInterfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueCorpOrder.Application.QueryHandler
{
    public class ReadyForDispatchQueryHandler(IReadyForDispatchRepository readyForDispatchRepository) : IRequestHandler<ReadyForDispatchQuery, Domain.Entities.ReadyForDispatch?>
    {
        public Task<ReadyForDispatch?> Handle(ReadyForDispatchQuery request, CancellationToken cancellationToken)
        {
            return readyForDispatchRepository.GetReadyForDispatchByControlNumberAsync(request.readyForDispatch.ControlNumber);
        }
    }
}
