using System;

namespace Source.Options;

public class SmtpOption
{
    public const string Smtp = "Smtp";
    public required string Host { get; set; }
    public required int Port { get; set; }
    public required string FromEmail { get; set; }
}
