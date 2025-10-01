using Business.Services.Business;
using Data.Interfaz.DataBasic;
using Data.Interfaz.IDataImplement.Business;
using Entity.Domain.Models.Implements.AdministrationSystem;
using Entity.Domain.Models.Implements.Business;
using MapsterMapper;
using Moq;
using Utilities.Exceptions;

namespace Test.Modulo.Business;

public class ObligationMonthServiceGenericTests
{
    private readonly Mock<IObligationMonthRepository> _obligationRepo = new();
    private readonly Mock<IContractRepository> _contractRepo = new();
    private readonly Mock<IDataGeneric<SystemParameter>> _systemParams = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly ObligationMonthService _service;

    public ObligationMonthServiceGenericTests()
    {
        _service = new ObligationMonthService(
            _obligationRepo.Object,
            _contractRepo.Object,
            _systemParams.Object,
            _mapper.Object
        );
    }

    [Fact]
    public async Task GetTotalDay_Throws_WhenDateIsDefault()
    {
        var ex = await Assert.ThrowsAsync<BusinessException>(() =>
            _service.GetTotalObligationsPaidByDayAsync(default));

        Assert.Contains("fecha no es válida", ex.InnerException!.Message);
    }

    [Fact]
    public async Task GetTotalMonth_Throws_WhenYearOrMonthInvalid()
    {
        var ex = await Assert.ThrowsAsync<BusinessException>(() =>
            _service.GetTotalObligationsPaidByMonthAsync(0, 13));

        Assert.Contains("año o mes inválido", ex.InnerException!.Message);
    }

    [Fact]
    public async Task GetTotalDay_ReturnsDecimal_WhenValid()
    {
        // Arrange
        _obligationRepo.Setup(r => r.GetTotalObligationsPaidByDayAsync(DateTime.Today))
                       .ReturnsAsync(3000m);

        // Act
        var total = await _service.GetTotalObligationsPaidByDayAsync(DateTime.Today);

        // Assert
        Assert.Equal(3000m, total);
    }

    [Fact]
    public async Task GetTotalMonth_ReturnsDecimal_WhenValid()
    {
        // Arrange
        _obligationRepo.Setup(r => r.GetTotalObligationsPaidByMonthAsync(2025, 10))
                       .ReturnsAsync(4500m);

        // Act
        var total = await _service.GetTotalObligationsPaidByMonthAsync(2025, 10);

        // Assert
        Assert.Equal(4500m, total);
    }
}
