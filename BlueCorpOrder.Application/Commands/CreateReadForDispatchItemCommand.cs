using BlueCorpOrder.Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueCorpOrder.Application.Commands
{
    public class CreateReadForDispatchItemCommand : IRequest<int>
    {
        public required ReadyForDispatchDto readyForDispatch {  get; set; }
    }
}
