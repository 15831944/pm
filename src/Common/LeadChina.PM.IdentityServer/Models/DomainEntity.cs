﻿using System;

namespace LeadChina.PM.Domain.Models
{
    public abstract class DomainEntity<TKey>
    {
        public TKey Id { get; }

        protected DomainEntity(TKey id)
        {
            if (id.Equals(default(TKey)))
            {
                throw new ArgumentOutOfRangeException(nameof(id), "The identifier cannot be equal to the default value of the type.");
            }

            Id = id;
        }

        public override bool Equals(object obj)
        {
            var entity = obj as DomainEntity<TKey>;

            if (entity == null)
            {
                return false;
            }
            else
            {
                return Id.Equals(entity.Id);
            }
        }

        public static bool operator ==(DomainEntity<TKey> x, DomainEntity<TKey> y)
        {
            if ((object)x == null)
            {
                return (object)y == null;
            }

            if ((object)y == null)
            {
                return (object)x == null;
            }

            return x.Id.Equals(y.Id);
        }

        public static bool operator !=(DomainEntity<TKey> x, DomainEntity<TKey> y)
        {
            return !(x == y);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}