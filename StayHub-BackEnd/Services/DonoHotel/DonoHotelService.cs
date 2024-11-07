using StayHub_BackEnd.Data;

namespace StayHub_BackEnd.Services.DonoHotel
{
    public class DonoHotelService : IDonoHotel
    {
        private readonly AppDbContext _context;
        public DonoHotelService(AppDbContext context)
        {
            _context = context;
        }


    }
}
