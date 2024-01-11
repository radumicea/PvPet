using FirebaseAdmin.Messaging;
using Microsoft.EntityFrameworkCore;
using PvPet.Data.Contexts;

namespace PvPet.Business.Services;

public class NotificationService
{
    private readonly PvPetDbContext _context;

    public NotificationService(PvPetDbContext context)
    {
        _context = context;
    }

    public async Task NotifyPetOwner(Guid petId, string title)
    {
        var user = await _context.Users.Include(u => u.Pet).SingleAsync(u => u.Pet.Id == petId);

        if (user.FirebaseToken is not null)
        {
            await FirebaseMessaging.DefaultInstance.SendAsync(new Message
            {
                Token = user.FirebaseToken,
                Data = new Dictionary<string, string>
                {
                    {"title", title}
                }
            });
        }
    }
}
