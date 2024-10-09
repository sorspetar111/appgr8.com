using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGatewayAPI.Services
{

	public class CountryService : ICountryService
	{
		private readonly PaymentGatewayDbContext _context;		


		public CountryService(PaymentGatewayDbContext context)
		{
			_context = context;			
		}

	}

}