namespace TotpGenerator.Pages;

public partial class Index
{
    private readonly Timer timer = new(1);

    private Totp? totp = null;

    private string secret = string.Empty;

    private string otpCode = string.Empty;
    private string remainingTime = string.Empty;

    protected override void OnInitialized() => timer.Elapsed += TimerElapsed;

    private void SetSecret()
    {
        timer.Stop();
        totp = new
        (
            secretKey: Base32Encoding.ToBytes(secret),
            mode: OtpHashMode.Sha1,
            step: 30,
            totpSize: 6
        );
        timer.Start();
    }

    private void TimerElapsed(object? sender, ElapsedEventArgs e) => GetOtpCode();

    private void GetOtpCode()
    {
        if (totp is null)
        {
            return;
        }
        otpCode = $"{totp.ComputeTotp(DateTime.UtcNow):000 000}";
        remainingTime = $"{totp.RemainingSeconds()} second(s)";
        StateHasChanged();
    }
}
