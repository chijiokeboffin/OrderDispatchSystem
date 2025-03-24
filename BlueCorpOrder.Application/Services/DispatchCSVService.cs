using BlueCorpOrder.Application.Dtos;
using BlueCorpOrder.Application.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueCorpOrder.Application.Services
{
    public class DispatchCSVService: IDispatchCSVService
    {
        public string ConvertObjectToCsv(ReadyForDispatchDto dispatchDto)
        {
            var csvFile = new StringBuilder();
            csvFile.AppendLine("CustomerReference,LoadId,ContainerType,ItemCode,ItemQuantity,ItemWeight,Street,City,State,PostalCode,Country");
            foreach (var container in dispatchDto.Containers)
            {
                string containerType = container.ContainerType switch
                {
                    "20RF" => "REF20",
                    "40RF" => "REF40",
                    "20HC" => "HC20",
                    "40HC" => "HC40",
                    _ => throw new ArgumentException($"Invalid container type: {container.ContainerType}")
                };

                foreach (var item in container.Items)
                {
                    csvFile.AppendLine($"{dispatchDto.SalesOrder},{container.LoadId},{containerType},{item.ItemCode},{item.Quantity},{item.CartonWeight},{dispatchDto.DeliveryAddress.Street},{dispatchDto.DeliveryAddress.City},{dispatchDto.DeliveryAddress.State},{dispatchDto.DeliveryAddress.PostalCode},{dispatchDto.DeliveryAddress.Country}");
                }
            }

            return csvFile.ToString();
        }
    }
}
