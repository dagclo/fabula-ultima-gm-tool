using FabulaUltimaNpc;

public interface ISearchFilter<TTargetType>
{
	bool Apply(TTargetType searchType);
}
