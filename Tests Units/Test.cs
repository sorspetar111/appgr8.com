


using Moq;
using Xunit;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;

public class GatewayControllerTests
{
    private readonly Mock<PaymentGatewayDbContext> _mockContext;

    public GatewayControllerTests()
    {
        _mockContext = new Mock<PaymentGatewayDbContext>();
    }

    [Theory]
    [InlineData(0, "Approved")]
    [InlineData(1, "Failed")]
    [InlineData(2, "Wrong Card ID")]
    [InlineData(3, "Expired Credit Card")]
    [InlineData(4, "Insufficient Funds")]
    public async Task SimulateBankResponseAsync_ShouldUpdateTransactionStatus_Correctly(int randomOutcome, string expectedStatus)
    {
        // Arrange
        var transaction = new Transaction { TransactionStatus = "Pending" };
        _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        
        var controller = new GatewayController(_mockContext.Object, null, null);
        var random = new Mock<Random>();
        
        // Mock the Random.Next() method to control its outcome
        random.Setup(r => r.Next(0, 4)).Returns(randomOutcome);

        // Act
        // await controller.SimulateBankResponseAsync(transaction);
        await SimulateBankResponseAsync(transaction);

        // Assert
        Assert.Equal(expectedStatus, transaction.TransactionStatus);
        _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }


    private async Task SimulateBankResponseAsync(Transaction transaction)
    {
       
        await Task.Delay(5000);
        
        
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

}




