using System;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using FunctionAppContagem.Models;

namespace FunctionAppContagem;

public static class ValorAtual
{
    private static ConnectionMultiplexer _redisConnection =
        ConnectionMultiplexer.Connect(
           Environment.GetEnvironmentVariable("Redis"));

    [Function(nameof(ValorAtual))]
    [OpenApiOperation(operationId: "Contagem", tags: new[] { "Juros" })]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ResultadoContador), Description = "Resultado da contagem de acessos ao Redis")]
    public static HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req,
        FunctionContext executionContext)
    {
        var logger = executionContext.GetLogger(nameof(ValorAtual));
        logger.LogInformation("Requisição HTTP recebida...");

        var valorAtualContador = Convert.ToInt32(
            _redisConnection.GetDatabase().StringIncrement("ContadorAzureFunctions"));

        if (Convert.ToBoolean(Environment.GetEnvironmentVariable("SimularFalha")) &&
            valorAtualContador % 4 == 0)
        {
            logger.LogError("Simulando falha...");
            throw new Exception("Simulação de falha!");
        }

        logger.LogInformation($"Contador - Valor atual: {valorAtualContador}");

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.WriteAsJsonAsync(new ResultadoContador(valorAtualContador));
        return response;
    }
}