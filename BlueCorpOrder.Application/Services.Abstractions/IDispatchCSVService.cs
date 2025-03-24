using BlueCorpOrder.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueCorpOrder.Application.Services.Abstractions
{
    public interface IDispatchCSVService
    {
        string ConvertObjectToCsv(ReadyForDispatchDto dispatchDto);
    }
}
