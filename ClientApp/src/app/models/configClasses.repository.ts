

export class Filter
{
	category?: string;
	search?: string;
	related: boolean = false;

	reset()
	{
		this.category = null;
		this.search = null;
		this.related = false;
	}
}