using AutoMapper;
using FieldOps.Application.DTOs;
using FieldOps.Domain.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldOps.Application.MappingProfiles;

public class ClientProfile : Profile
{
    public ClientProfile()
    {
        // Domain to DTO (for GET requests)
        CreateMap<Client, ClientDTO>();
        // DTO to Domain (for POST requests)
        CreateMap<ClientDTO, Client>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
    }
}