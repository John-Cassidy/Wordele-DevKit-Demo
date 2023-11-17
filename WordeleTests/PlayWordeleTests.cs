using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using Wordele.Pages;
using WordeleLogic;
using RichardSzalay.MockHttp;

namespace WordeleTests;

public class PlayWordeleTests : TestContext
{
    [Fact]
    public void AttemptsStartAtOne()
    {
        var mock = Services.AddMockHttpClient();
        mock.When("sample-data/codele-word-library.json").RespondJson(new string[2] { "hello", "world" });
        var cut = RenderComponent<PlayWordele>();

        cut.Find("strong").MarkupMatches("<strong>Attempt #: 1</strong>");
    }

    [Fact]
    public void SubmittingWrongGuess()
    {
        var mock = Services.AddMockHttpClient();
        string[] answers = new string[2] { "hello", "world" };
        mock.When("sample-data/codele-word-library.json").RespondJson(answers);
        var cut = RenderComponent<PlayWordele>(parameters => parameters.Add(p => p.answers, answers).Add(p => p.answer, "world"));

        cut.Find("input").Change("hello");

        cut.Find("button").Click();

        cut.Find("strong").MarkupMatches("<strong>Attempt #: 2</strong>");
    }

    [Fact]
    public void SubmittingCorrectGuess()
    {
        var mock = Services.AddMockHttpClient();
        string[] answers = new string[2] { "hello", "world" };
        mock.When("sample-data/codele-word-library.json").RespondJson(answers);
        var cut = RenderComponent<PlayWordele>(parameters => parameters.Add(p => p.answers, answers).Add(p => p.answer, "world"));

        cut.Find("input").Change("world");

        cut.Find("button").Click();

        cut.Find("strong").MarkupMatches("<strong>Attempt #: 1</strong>");
    }

    [Fact]
    public void GuessIsNotFiveLetters()
    {
        var mock = Services.AddMockHttpClient();
        string[] answers = new string[2] { "hello", "world" };
        mock.When("sample-data/codele-word-library.json").RespondJson(answers);
        var cut = RenderComponent<PlayWordele>(parameters => parameters.Add(p => p.answers, answers).Add(p => p.answer, "hello"));

        cut.Find("input").Change("worlds");

        cut.Find("button").Click();

        var pTags = cut.FindAll("p");
        pTags[2].MarkupMatches(@"<p style=""color: rgb(197, 3, 3);"">Guess must be 5 characters long</p>");
    }

    [Fact]
    public void SubmittingCorrectGuessWithMultipleAnswers()
    {
        var mock = Services.AddMockHttpClient();
        string[] answers = new string[3] { "hello", "world", "foo" };
        mock.When("sample-data/codele-word-library.json").RespondJson(answers);
        var cut = RenderComponent<PlayWordele>(parameters => parameters.Add(p => p.answers, answers).Add(p => p.answer, "world"));

        cut.Find("input").Change("world");

        cut.Find("button").Click();

        cut.Find("strong").MarkupMatches("<strong>Attempt #: 1</strong>");
    }

    [Fact]
    public void SubmittingWrongGuessWithMultipleAnswers()
    {
        var mock = Services.AddMockHttpClient();
        string[] answers = new string[3] { "hello", "world", "foo" };
        mock.When("sample-data/codele-word-library.json").RespondJson(answers);
        var cut = RenderComponent<PlayWordele>(parameters => parameters.Add(p => p.answers, answers).Add(p => p.answer, "world"));

        cut.Find("input").Change("hello");

        cut.Find("button").Click();

        cut.Find("strong").MarkupMatches("<strong>Attempt #: 2</strong>");
    }
}
