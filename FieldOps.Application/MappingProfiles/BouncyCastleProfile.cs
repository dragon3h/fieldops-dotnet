using AutoMapper;
using FieldOps.Application.DTOs;
using FieldOps.Domain.BouncyCastle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldOps.Application.MappingProfiles;

public class BouncyCastleProfile : Profile
{
    public BouncyCastleProfile()
    {
        // Domain to DTO (for GET requests)
        CreateMap<BouncyCastle, BouncyCastleDTO>();

        // DTO to Domain (for POST requests)
        CreateMap<BouncyCastleDTO, BouncyCastle>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
    }
}