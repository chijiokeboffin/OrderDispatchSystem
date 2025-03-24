using BlueCorpOrder.Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueCorpOrder.Application.Query
{
    public class ReadyForDispatchQuery : IRequest<BlueCorpOrder.Domain.Entities.ReadyForDispatch>
    {
        public required ReadyForDispatchDto readyForDispatch { get; set; }
    }
}
