using FabulaUltimaNpc;
using System;
using System.Collections.Generic;
using System.Linq;

public class SearchFilter<TTargetType> : ISearchFilter<TTargetType>
{
	public Func<TTargetType, bool> Filter { get; }
    public SearchFilter(Func<TTargetType, bool> filter)
	{
		Filter = filter ?? throw new ArgumentNullException("set filter");
	}
	public bool Apply(TTargetType target) => Filter.Invoke(target);
}


public class CompositeSearchFilter<TTargetType> : ISearchFilter<TTargetType>
{
	public ICollection<ISearchFilter<TTargetType>> Filters { get; } = new List<ISearchFilter<TTargetType>>();

	public bool Apply(TTargetType target)
	{
		if (!Filters.Any()) return true;
		foreach(var filter in Filters)
		{
			var result = filter.Apply(target);
			if (!result) return false;
		}
		return true;
	}
}