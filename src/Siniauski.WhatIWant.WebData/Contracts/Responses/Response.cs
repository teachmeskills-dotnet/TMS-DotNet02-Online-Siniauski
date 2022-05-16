using Siniauski.WhatIWant.WebData.Enums;

namespace Siniauski.WhatIWant.WebData.Contracts.Responses
{
    public class Response
    {
        public ResponseStatus Status { get; set; }
        public string? Message { get; set; }
    }
}