using System;

namespace LiveLink.Services.DuplicatesService
{
	public class TextComparisonService : ITextComparisonService
	{
		public double PercentageSimilarity(string a, string b)
		{
			return 1.0 - Compare(a, b) / (double)Math.Max(a.Length, b.Length);
		}

        // TODO: Rewrite this into something that doesn't use awful variable names
        // TODO: Write some tests for this, its very algorithmic
        // This looks like Edit Distance
		private static int Compare(string s, string t)
		{
			if (string.IsNullOrEmpty(s))
			{
			    return string.IsNullOrEmpty(t) ? 0 : t.Length;
			}

			if (string.IsNullOrEmpty(t))
			{
				return s.Length;
			}

			var n = s.Length;
		    var m = t.Length;
			var d = new int[n + 1, m + 1];
            
			for (var i = 0; i <= n; d[i, 0] = i++);
			for (var j = 1; j <= m; d[0, j] = j++);

			for (var i = 1; i <= n; i++)
			{
				for (var j = 1; j <= m; j++)
				{
				    var cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
				    var min1 = d[i - 1, j] + 1;
				    var min2 = d[i, j - 1] + 1;
				    var min3 = d[i - 1, j - 1] + cost;
					d[i, j] = Math.Min(Math.Min(min1, min2), min3);
				}
			}
			return d[n, m];
		}
	}
}