﻿namespace EventApp.Entities;

public class PackageEntity
{
    public int Id { get; set; }
    public string Name { get; set; }   = string.Empty;

    public int EventId { get; set; }
    public EventEntity Event { get; set; } = null!;
}
