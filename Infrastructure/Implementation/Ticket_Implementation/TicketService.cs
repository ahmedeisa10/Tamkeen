using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Errors.Model;
using Tamkeen.Application.DTOs.Ticket_DTOs;
using Tamkeen.Application.Interfaces.Ticket_Interface;
using Tamkeen.Domain.Entities;
using Tamkeen.Domain.Enums;
using Tamkeen.Infrastructure.Data;
using Tamkeen.Infrastructure.Services;

namespace Tamkeen.Infrastructure.Implementation.Ticket_Implementation
{
    public class TicketService : ITicketService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IImageService imageService;

        public TicketService(AppDbContext context, IMapper mapper, IImageService imageService)
        {
            _context = context;
            _mapper = mapper;
            this.imageService = imageService;
        }

        public async Task<TicketResponseDto> CreateAsync(CreateTicketDto dto, string tenantId)
        {
            var ticket = new Ticket
            {
                Id = Guid.NewGuid(),
                Description = dto.Description,
                Priority = dto.Priority,
                Arrival = dto.Arrival,
                Deadline = dto.Deadline,
                CompanyId = dto.CompanyId,
                TenantId = tenantId,
                Status = RequestStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            // لو في صور يحفظهم
            if (dto.Images != null && dto.Images.Any())
            {

                ticket.Images = new List<Image>();
                foreach (var file in dto.Images)
                {

                    var url = await imageService.SaveImageAsync(file, "tickets");
                    ticket.Images.Add(new Image
                    {
                        Id = Guid.NewGuid(),
                        Url = url,
                        Type = ImageType.Before,
                        TicketId = ticket.Id
                    });
                }
            }

            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();

            return _mapper.Map<TicketResponseDto>(ticket);
        }

        public async Task<TicketResponseDto> GetByIdAsync(Guid id, string userId, string role)
        {
            var ticket = await _context.Tickets
                .Include(t => t.Tenant)
                .Include(t => t.Vendor)
                .Include(t => t.Images)
                .FirstOrDefaultAsync(t => t.Id == id)
                ?? throw new NotFoundException("Ticket not found");

            // Manager يشوف أي ticket
            if (role == "Manager")
                return _mapper.Map<TicketResponseDto>(ticket);

            // Tenant يشوف بتاعته بس
            if (role == "Tenant" && ticket.TenantId != userId)
                throw new ForbiddenException("Access denied");

            // Vendor يشوف اللي assigned له بس
            if (role == "Vendor" && ticket.VendorId != userId)
                throw new ForbiddenException("Access denied");

            return _mapper.Map<TicketResponseDto>(ticket);
        }

        public async Task<IEnumerable<TicketResponseDto>> GetAllAsync(string userId, string role)
        {
            var query = _context.Tickets
                .Include(t => t.Tenant)
                .Include(t => t.Vendor)
                .Include(t => t.Images)
                .AsQueryable();

            query = role switch
            {
                "Tenant" => query.Where(t => t.TenantId == userId),
                "Vendor" => query.Where(t => t.VendorId == userId),
                _ => query // Manager يشوف الكل
            };

            var tickets = await query.ToListAsync();
            return _mapper.Map<IEnumerable<TicketResponseDto>>(tickets);
        }


        public async Task AssignVendorAsync(Guid id, AssignTicketDto dto)
        {
            var ticket = await _context.Tickets.FindAsync(id)
                ?? throw new NotFoundException("Ticket not found");

            if (ticket.Status != RequestStatus.Pending)
                throw new BadRequestException("Only pending tickets can be assigned");

            ticket.VendorId = dto.VendorId;
            ticket.Status = RequestStatus.Assigned;
            await _context.SaveChangesAsync();
        }

        public async Task AcceptAsync(Guid id, string vendorId)
        {
            var ticket = await _context.Tickets.FindAsync(id)
                ?? throw new NotFoundException("Ticket not found");

            if (ticket.VendorId != vendorId)
                throw new ForbiddenException("Not your ticket");

            if (ticket.Status != RequestStatus.Assigned)
                throw new BadRequestException("Ticket must be assigned first");

            ticket.Status = RequestStatus.InProgress;
            await _context.SaveChangesAsync();
        }

        public async Task RejectAsync(Guid id, string vendorId)
        {
            var ticket = await _context.Tickets.FindAsync(id)
                ?? throw new NotFoundException("Ticket not found");

            if (ticket.VendorId != vendorId)
                throw new ForbiddenException("Not your ticket");

            if (ticket.Status != RequestStatus.Assigned)
                throw new BadRequestException("Ticket must be assigned first");

            // يرجع Pending وينزع الـ Vendor
            ticket.Status = RequestStatus.Pending;
            ticket.VendorId = null;
            await _context.SaveChangesAsync();
        }

        public async Task CompleteAsync(Guid id, string vendorId)
        {
            var ticket = await _context.Tickets.FindAsync(id)
                ?? throw new NotFoundException("Ticket not found");

            if (ticket.VendorId != vendorId)
                throw new ForbiddenException("Not your ticket");

            if (ticket.Status != RequestStatus.InProgress)
                throw new BadRequestException("Ticket must be in progress first");

            ticket.Status = RequestStatus.Resolved;
            await _context.SaveChangesAsync();
        }

        public async Task CloseAsync(Guid id, string tenantId)
        {
            var ticket = await _context.Tickets.FindAsync(id)
                ?? throw new NotFoundException("Ticket not found");

            if (ticket.TenantId != tenantId)
                throw new ForbiddenException("Not your ticket");

            if (ticket.Status != RequestStatus.Resolved)
                throw new BadRequestException("Ticket must be resolved first");

            ticket.Status = RequestStatus.Closed;
            await _context.SaveChangesAsync();
        }

        public async Task<List<ImageResponseDto>> UploadImagesAsync(Guid ticketId, UploadTicketImagesDto dto, string userId)
        {
            var ticket = await _context.Tickets
                .Include(t => t.Images)
                .FirstOrDefaultAsync(t => t.Id == ticketId)
                ?? throw new NotFoundException("Ticket not found");

            // Tenant يرفع Before فقط، Vendor يرفع After فقط
            if (dto.Type == ImageType.Before && ticket.TenantId != userId)
                throw new ForbiddenException("Only tenant can upload before images");

            if (dto.Type == ImageType.After && ticket.VendorId != userId)
                throw new ForbiddenException("Only vendor can upload after images");

            var savedImages = new List<Image>();

            foreach (var file in dto.Images)
            {
                var url = await imageService.SaveImageAsync(file, "tickets");

                var image = new Image
                {
                    Id = Guid.NewGuid(),
                    Url = url,
                    Type = dto.Type,
                    TicketId = ticketId
                };

                savedImages.Add(image);
            }

            await _context.Images.AddRangeAsync(savedImages);
            await _context.SaveChangesAsync();

            return _mapper.Map<List<ImageResponseDto>>(savedImages);
        }
    }

}
