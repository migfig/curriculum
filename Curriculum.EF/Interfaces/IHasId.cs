using System;

namespace Curriculum.EF.Models;

public interface IHasId
{
    Guid Id { get; set; }
}
