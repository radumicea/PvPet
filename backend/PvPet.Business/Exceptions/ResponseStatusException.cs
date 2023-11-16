using System.Net;

namespace PvPet.Business.Exceptions;

internal class ResponseStatusException : Exception
{
    public HttpStatusCode StatusCode { get; }

    public ResponseStatusException(HttpStatusCode statusCode)
    {
        StatusCode = statusCode;
    }

    public ResponseStatusException(HttpStatusCode statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
    }
}