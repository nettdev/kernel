﻿using System.Diagnostics.CodeAnalysis;
using MediatR;

namespace Mobnet.SharedKernel;

[ExcludeFromCodeCoverage]
public class Entity : IEquatable<Entity>
{
    private List<INotification> _events;
    public IReadOnlyCollection<INotification> Events => _events.AsReadOnly();

    public Guid Id { get; set; }

    protected Entity() 
    {
        Id = Guid.NewGuid();
        _events = new List<INotification>();
    }

    public void AddEvent(INotification @event) =>
        _events.Add(@event);

    public void RemoveEvent(INotification @event) =>
        _events.Remove(@event);

    public void ClearEvents() =>
        _events.Clear();

    public static bool operator == (Entity a, Entity b) =>
        a.Equals(b);

    public static bool operator != (Entity a, Entity b) =>
        !(a == b);

    public override int GetHashCode() =>
        (GetType().GetHashCode() ^ 93) + Id.GetHashCode();

    public bool Equals(Entity? other) =>
        Id == other?.Id;

    public override bool Equals(object? obj) =>
        obj is Entity entity && Id == entity.Id;
}