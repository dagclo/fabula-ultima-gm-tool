public interface ISearchFilter<TTargetType>
{
	bool Apply(TTargetType searchType);
}
