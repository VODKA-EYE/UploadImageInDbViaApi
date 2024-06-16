using System;
using System.Collections.Generic;

namespace ImageInDbApi.Models;

public partial class DbImage
{
    public int DbImageId { get; set; }

    public byte[] DbImageBytea { get; set; } = null!;
}
