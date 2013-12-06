// Source code: http: //jqueryui.com/demos/autocomplete/

(function ($) {
	$.widget("ui.combobox",
	{
		_create: function () {
			var input, a,
			hintId,
			self = this, select = this.element.hide(), selected = select.children(":selected"), value = selected.val() ? selected.text() : "", wrapper = this.wrapper = $("<span>").addClass("ui-combobox").insertAfter(select);

			input = this.input = $("<input>").appendTo(wrapper).val(value).addClass("ui-state-default ui-combobox-input").autocomplete({
				autoFocus: true,
				delay: 0,
				minLength: 0,
				source: function (request, response) {
					// Customize by Kaden, set value input to attribute temporary so that we can validation before submit
					input.attr("temp-text", request.term);
					// End customize

					var matcher = new RegExp($.ui.autocomplete.escapeRegex(request.term), "i");
					response(select.children("option").map(function () {
						var text = $(this).text();
						if (this.value && (!request.term || matcher.test(text)))
							return {
								label: text.replace(new RegExp("(?![^&;]+;)(?!<[^<>]*)(" + $.ui.autocomplete.escapeRegex(request.term) + ")(?![^<>]*>)(?![^&;]+;)", "gi"), "<strong>$1</strong>"),
								value: text,
								option: this
							};
						// Customize by Kaden, hide tooltip wrong value on select
						Validators.hide();
					}));
				},

				select: function (event, ui) {
					ui.item.option.selected = true;
					self._trigger("selected", event,
					{
						item: ui.item.option
					});

					// Customize by Jacob, need to blur after select
					$(this).blur();
					$('#' + select.context.id).focus(); // For fix bug IE8
					input.attr("temp-text", ui.item.value);
				},

				change: function (event, ui) {
					if (!ui.item) {
						var matcher = new RegExp("^" + $.ui.autocomplete.escapeRegex($(this).val()) + "$", "i"),
						valid = false;

						select.children("option").each(function () {
							if ($(this).text().match(matcher)) {
								this.selected = valid = true;
								return false;
							}
						});

						// Customize by Kaden, set value input to attribute temporary so that we can validation before submit
						input.attr("temp-text", $(this).val());
						if (!valid) {
							// Customize by Kaden, show tooltip wrong value on select
							hintId = this.id;
							return false;
						}
					}
					ui.item.option.selected = true;
					self._trigger("selected", event,
										{
											item: ui.item.option
										});
				}
			}).addClass("ui-widget ui-widget-content ui-corner-left");

			input.data("autocomplete")._renderItem = function (ul, item) {
				return $("<li></li>").data("item.autocomplete", item).append("<a>" + item.label + "</a>").appendTo(ul);
			};

			a = this.a = $("<a>").attr("tabIndex", -1).attr("title", "Show All Items").appendTo(wrapper).button({
				icons:
				{
					primary: "ui-icon-triangle-1-s"
				},
				text: false
			}).removeClass("ui-corner-all").attr("id", "a-" + select.context.id).addClass("ui-corner-right ui-combobox-toggle").click(function () {
				// close if already visible
				if (input.autocomplete("widget").is(":visible")) {
					input.autocomplete("close");
					return;
				}

				// Customize by Kaden, hide tooltip wrong value on this select
				oldValue = input.attr("temp-text");
				Validators.hide();
				// work around a bug (likely same cause as #5265)
				$(this).blur();

				// pass empty string as value to search for, displaying all results
				input.autocomplete("search", "");
				input.focus();

				// Customize by Kaden, hide tooltip wrong value on this select
				input.attr("temp-text", oldValue);
			});

			// Customize by Jacob
			this.customize(input, select);
		},

		destroy: function () {
			this.wrapper.remove();
			this.element.show();

			$.Widget.prototype.destroy.call(this);
		},
		disable: function () {
			this.input.prop('disabled', true);
			this.input.autocomplete("disable");
			this.a.button("disable");
		},
		enable: function () {
			this.input.prop('disabled', false);
			this.input.autocomplete("enable");
			this.a.button("enable");
		},
		// Customize by Jacob,
		customize: function (input, select) {
			// Focus and Focusout
			input.attr("selected-text", select.children(":selected").text());
			input.attr("temp-text", select.children(":selected").text());
			input.focus(function (eventObject) {
				input.attr("selected-text", select.children(":selected").text());
				if (typeof (eventObject.isTrigger) == 'undefined' && input.is(":focus")) {
					$(this).val("");
				}
			});
			input.focusout(function (eventObject) {
				var currentValue = $(this).val();
				currentValue = $.trim(currentValue);
				if (currentValue == "" && typeof ($(this).attr("temp-text")) !== 'undefined') {
					$(this).val($(this).attr("temp-text"));
				}
				else if (currentValue == "") {
					$(this).val($(this).attr("selected-text"));
				}
			});

			// Customize css class
			var id = select.context.id;
			if (typeof (this.element.context.attributes["class"]) !== 'undefined') {
				var className = this.element.context.attributes["class"].value;
				input.removeClass("ui-state-default");
				input.addClass(className);
				input.attr("id", "input-" + id);
			}
		}
	});
})(jQuery);