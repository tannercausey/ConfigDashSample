/*
 * Adapted from: https://github.com/dotnet/aspnetcore/blob/master/src/Components/Web/src/Routing/NavLink.cs
 */
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Routing;
using System.Diagnostics;
using Telerik.Blazor.Components;

namespace ConfigDashSample.Client.Shared.Layout;

public class DrawerNavLink : ComponentBase, IDisposable
{
	/// <summary>
	/// A component that renders an anchor tag, automatically toggling its 'active'
	/// class based on whether its 'href' matches the current URI.
	/// </summary>
	private const string DefaultActiveClass = "k-state-selected";

	private bool _isActive;
	private string? _hrefAbsolute;
	private string? _class = "k-drawer-item";

	/// <summary>
	/// Gets or Sets the NavLink Text to display.
	/// </summary>
	[Parameter] public string? Text { get; set; }

	/// <summary>
	/// Gets or Sets the NavLink Icon to display.
	/// </summary>
	[Parameter] public string? Icon { get; set; }

	[Parameter] public int? Level { get; set; }

	[Parameter] public bool ExpandedDrawer { get; set; }

	/// <summary>
	/// Gets or sets the CSS class name applied to the NavLink when the
	/// current route matches the NavLink href.
	/// </summary>
	[Parameter]
	public string? ActiveClass { get; set; }

	/// <summary>
	/// Gets or sets a collection of additional attributes that will be added to the generated
	/// <c>a</c> element.
	/// </summary>
	[Parameter(CaptureUnmatchedValues = true)]
	public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

	/// <summary>
	/// Gets or sets the computed CSS class based on whether or not the link is active.
	/// </summary>
	protected string? CssClass { get; set; }

	/// <summary>
	/// Gets or sets a value representing the URL matching behavior.
	/// </summary>
	[Parameter]
	public NavLinkMatch Match { get; set; }

	[Inject] private NavigationManager NavigationManger { get; set; } = default!;

	/// <inheritdoc />
	protected override void OnInitialized()
	{
		// We'll consider re-rendering on each location change
		NavigationManger.LocationChanged += OnLocationChanged;
	}

	/// <inheritdoc />
	protected override void OnParametersSet()
	{
		// Update computed state
		string? href = null;
		if (AdditionalAttributes != null && AdditionalAttributes.TryGetValue("href", out object? obj))
		{
			href = Convert.ToString(obj);
		}

		_hrefAbsolute = href == null ? null : NavigationManger.ToAbsoluteUri(href).AbsoluteUri;
		_isActive = ShouldMatch(NavigationManger.Uri);

		UpdateCssClass();
	}

	/// <inheritdoc />
	public void Dispose()
	{
		// To avoid leaking memory, it's important to detach any event handlers in Dispose()
		NavigationManger.LocationChanged -= OnLocationChanged;
	}

	private void UpdateCssClass()
	{
		string levelClass = "k-level-" + Level.ToString();
		string? comboClass = CombineWithSpace(_class, levelClass);
		CssClass = _isActive ? CombineWithSpace(comboClass, ActiveClass ?? DefaultActiveClass) : comboClass;
	}

	private void OnLocationChanged(object? sender, LocationChangedEventArgs args)
	{
		// We could just re-render always, but for this component we know the
		// only relevant state change is to the _isActive property.
		bool shouldBeActiveNow = ShouldMatch(args.Location);
		if (shouldBeActiveNow != _isActive)
		{
			_isActive = shouldBeActiveNow;
			UpdateCssClass();
			StateHasChanged();
		}
	}

	private bool ShouldMatch(string currentUriAbsolute)
	{
		return _hrefAbsolute != null &&
			(EqualsHrefExactlyOrIfTrailingSlashAdded(currentUriAbsolute) ||
			Match == NavLinkMatch.Prefix &&IsStrictlyPrefixWithSeparator(currentUriAbsolute, _hrefAbsolute));
	}

	private bool EqualsHrefExactlyOrIfTrailingSlashAdded(string currentUriAbsolute)
	{	// I love using methods to do checks or operations but can this method be simplified and/or split into two?
		Debug.Assert(_hrefAbsolute != null);

		if (string.Equals(currentUriAbsolute, _hrefAbsolute, StringComparison.OrdinalIgnoreCase))
		{
			return true;
		}

		if (currentUriAbsolute.Length == _hrefAbsolute.Length - 1)
		{
			// Special case: highlight links to http://host/path/ even if you're
			// at http://host/path (with no trailing slash)
			//
			// This is because the router accepts an absolute URI value of "same
			// as base URI but without trailing slash" as equivalent to "base URI",
			// which in turn is because it's common for servers to return the same page
			// for http://host/vdir as they do for host://host/vdir/ as it's no
			// good to display a blank page in that case.
			if (_hrefAbsolute[_hrefAbsolute.Length - 1] == '/'
				&& _hrefAbsolute.StartsWith(currentUriAbsolute, StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
		}

		return false;
	}

	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		builder.OpenElement(0, "li");
		builder.OpenElement(1, "a");
		builder.AddMultipleAttributes(2, AdditionalAttributes);
		builder.AddAttribute(3, "class", CssClass);
		builder.AddAttribute(5, "tabindex", _isActive ? "0" : "1");
		builder.AddAttribute(6, "role", "menuitem");
		builder.AddAttribute(7, "aria-label", Text);
		builder.OpenComponent<TelerikIcon>(8);
		builder.AddAttribute(9, "Icon", Icon);
		builder.CloseComponent();
		if(ExpandedDrawer)
		{
			builder.OpenElement(10, "span");
			builder.AddAttribute(11, "class", "k-item-text");
			builder.AddContent(12, Text);
			builder.CloseElement();
		}

		builder.CloseElement();
		builder.CloseElement(); // why call twice ? I see we open list and anchor elements but do we need to do this?
	}

	private static string? CombineWithSpace(string? str1, string str2)
		=> str1 == null ? str2 : $"{str1} {str2}";

	private static bool IsStrictlyPrefixWithSeparator(string value, string prefix)
	{
		/*
		 *	This bool needs to be rewritten to be more easily understood
		 *	If you wish to accept the changes editorConfig suggests, reformat each conditon's
		 *	expression as shown above with EqualsHrefExactlyOrIfTrailingSlashAdded() with the || or && coming after each condition
		 *	prefix.Length is just as long as prefixLength. Do we need a new variable?
		*/
		int prefixLength = prefix.Length;
		if (value.Length > prefixLength)
		{
			string[] splitVal = value.Split('/');
			if (splitVal[1] == "")
			{
				return false;
			}
			return value.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
				&& (
					// Only match when there's a separator character either at the end of the
					// prefix or right after it.
					// Example: "/abc" is treated as a prefix of "/abc/def" but not "/abcdef"
					// Example: "/abc/" is treated as a prefix of "/abc/def" but not "/abcdef"
					prefixLength == 0
					|| !char.IsLetterOrDigit(prefix[prefixLength - 1])
					|| !char.IsLetterOrDigit(value[prefixLength])
				);
		}
		else
		{
			return false;
		}
	}
}