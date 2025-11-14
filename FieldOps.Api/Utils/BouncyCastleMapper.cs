using FieldOps.Application.Interfaces.IRepositories;
using FieldOps.Domain.BouncyCastle;
using FieldOps.Infrastructure.DTOs;

namespace FieldOps.Api.Utils;

public class BouncyCastleMapper : IMapper<BouncyCastle, BouncyCastleDTO>
{
    public BouncyCastleDTO Map(BouncyCastle source)
    {
        var dto = new BouncyCastleDTO
        {
            Id = source.Id,
            Name = source.Name,
            Description = source.Description,
            IsAvailable = source.IsAvailable,
            PurchasePrice = source.PurchasePrice,
            ResalePrice = source.ResalePrice,
            RentalPrice = source.RentalPrice,
            DepositPrice = source.DepositPrice,
            ProductType = source.ProductType,
            Capacity = source.Capacity,
            AgeFor = source.AgeFor,
            BouncyCastleType = source.BouncyCastleType,
            IsSplitUnit = source.IsSplitUnit,
            Size = source.Size,
            InflationTimeMinutes = source.InflationTimeMinutes,
            PumpIncluded = source.PumpIncluded
        };
        return dto;
    }

    public BouncyCastle MapForCreation(BouncyCastleDTO dtoObject)
    {
        var entity = new BouncyCastle
        {
            Name = dtoObject.Name,
            Description = dtoObject.Description,
            IsAvailable = dtoObject.IsAvailable,
            PurchasePrice = dtoObject.PurchasePrice,
            ResalePrice = dtoObject.ResalePrice,
            RentalPrice = dtoObject.RentalPrice,
            DepositPrice = dtoObject.DepositPrice,
            ProductType = dtoObject.ProductType,
            Capacity = dtoObject.Capacity,
            AgeFor = dtoObject.AgeFor,
            BouncyCastleType = dtoObject.BouncyCastleType,
            IsSplitUnit = dtoObject.IsSplitUnit,
            Size = dtoObject.Size,
            InflationTimeMinutes = dtoObject.InflationTimeMinutes,
            PumpIncluded = dtoObject.PumpIncluded
        };
        return entity;
    }

    public BouncyCastle MapForUpdate(BouncyCastle source, BouncyCastleDTO dtoObject)
    {
        source.Name = dtoObject.Name;
        source.Description = dtoObject.Description;
        source.IsAvailable = dtoObject.IsAvailable;
        source.PurchasePrice = dtoObject.PurchasePrice;
        source.ResalePrice = dtoObject.ResalePrice;
        source.RentalPrice = dtoObject.RentalPrice;
        source.DepositPrice = dtoObject.DepositPrice;
        source.ProductType = dtoObject.ProductType;
        source.Capacity = dtoObject.Capacity;
        source.AgeFor = dtoObject.AgeFor;
        source.BouncyCastleType = dtoObject.BouncyCastleType;
        source.IsSplitUnit = dtoObject.IsSplitUnit;
        source.Size = dtoObject.Size;
        source.InflationTimeMinutes = dtoObject.InflationTimeMinutes;
        source.PumpIncluded = dtoObject.PumpIncluded;

        return source;
    }

    public List<BouncyCastleDTO> MapList(List<BouncyCastle> source)
    {
        return source.Select(Map).ToList();
    }
}