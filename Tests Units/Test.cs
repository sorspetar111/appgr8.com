


private async Task SimulateBankResponseAsync(Transaction transaction)
{
    // Simulate bank response after 5 seconds (for demo purposes)
    await Task.Delay(5000);
    
    // Randomly assign a transaction status
    var random = new Random();
    var status = random.Next(0, 4) switch
    {
        0 => "Approved",
        1 => "Failed",
        2 => "Wrong Card ID",
        3 => "Expired Credit Card",
        _ => "Insufficient Funds"
    };

    transaction.TransactionStatus = status;
    await _context.SaveChangesAsync();
}
