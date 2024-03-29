using System;
using System.Runtime.InteropServices;

namespace FunctionAppContagem.Models;

public class ResultadoContador
{
    private static readonly string _SAUDACAO;
    private static readonly string _AVISO;
    private static readonly string _LOCAL;
    private static readonly string _KERNEL;
    private static readonly string _FRAMEWORK;

    static ResultadoContador()
    {
        _SAUDACAO = Environment.GetEnvironmentVariable("Saudacao");
        _AVISO = Environment.GetEnvironmentVariable("Aviso");
        _LOCAL = Environment.MachineName;
        _KERNEL = Environment.OSVersion.VersionString;
        _FRAMEWORK = RuntimeInformation.FrameworkDescription;
    }

    public int ValorAtual { get; init; }
    public string Saudacao { get => _SAUDACAO; }
    public string Aviso { get => _AVISO; }
    public string Local { get => _LOCAL; }
    public string Kernel { get => _KERNEL; }
    public string Framework { get => _FRAMEWORK; }

    public ResultadoContador(int valorAtual)
    {
        ValorAtual = valorAtual;
    }
}