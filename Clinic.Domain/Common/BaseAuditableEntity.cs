using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Domain.Common
{
	public abstract class BaseAuditableEntity
	{
		public DateTime CreatedAt { get; set; }
		public string? CreatedByUserId { get; set; }

		public DateTime? UpdatedAt { get; set; }
		public string? UpdatedByUserId { get; set; }

		public bool IsDeleted { get; set; } = false;
	}
}
