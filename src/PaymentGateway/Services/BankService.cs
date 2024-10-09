using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGatewayAPI.Services
{

	public class BankService : IBankService
	{
		private readonly PaymentGatewayDbContext _context;		


		public BankService(PaymentGatewayDbContext context)
		{
			_context = context;			
		}

	}

}