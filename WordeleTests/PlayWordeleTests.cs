using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using Wordele.Pages;
using WordeleLogic;
using RichardSzalay.MockHttp;

namespace WordeleTests;

public class PlayWordeleTests
{
    [Fact]
    public void AttemptsStartAtOne()
    {
        // Arrange
        using var ctx = new TestContext();

        // Act
        var mock = ctx.Services.AddMockHttpClient();
        mock.When("sample-data/wordele-word-library.json").RespondJson(new string[2] { "hello", "world" });
        var cut = ctx.RenderComponent<PlayWordele>();

        // Assert
        cut.Find("strong").MarkupMatches("<strong>Attempt #: 1</strong>");
    }

    [Fact]
    public void SubmittingWrongGuess()
    {
        // Arrange
        using var ctx = new TestContext();

        // Act
        var mock = ctx.Services.AddMockHttpClient();
        string[] answers = new string[2] { "hello", "world" };
        mock.When("sample-data/wordele-word-library.json").RespondJson(answers);
        var cut = ctx.RenderComponent<PlayWordele>(parameters => parameters.Add(p => p.answers, answers).Add(p => p.answer, "world"));

        cut.Find("input").Change("hello");

        cut.Find("button").Click();

        // Assert
        cut.Find("strong").MarkupMatches("<strong>Attempt #: 2</strong>");
    }

    [Fact]
    public void SubmittingCorrectGuess()
    {
        // Arrange
        using var ctx = new TestContext();

        // Act
        var mock = ctx.Services.AddMockHttpClient();
        string[] answers = new string[2] { "hello", "world" };
        mock.When("sample-data/wordele-word-library.json").RespondJson(answers);
        var cut = ctx.RenderComponent<PlayWordele>(parameters => parameters.Add(p => p.answers, answers).Add(p => p.answer, "world"));

        cut.Find("input").Change("world");

        cut.Find("button").Click();

        // Assert
        cut.Find("strong").MarkupMatches("<strong>Attempt #: 1</strong>");
    }

    [Fact]
    public void GuessIsNotFiveLetters()
    {
        // Arrange
        using var ctx = new TestContext();

        // Act
        var mock = ctx.Services.AddMockHttpClient();
        string[] answers = new string[2] { "hello", "world" };
        mock.When("sample-data/wordele-word-library.json").RespondJson(answers);
        var cut = ctx.RenderComponent<PlayWordele>(parameters => parameters.Add(p => p.answers, answers).Add(p => p.answer, "hello"));

        cut.Find("input").Change("worlds");

        cut.Find("button").Click();

        var pTags = cut.FindAll("p");

        // Assert
        pTags[2].MarkupMatches(@"<p style=""color: rgb(197, 3, 3);"">Guess must be 5 characters long</p>");
    }

    [Fact]
    public void SubmittingCorrectGuessWithMultipleAnswers()
    {
        // Arrange
        using var ctx = new TestContext();

        // Act
        var mock = ctx.Services.AddMockHttpClient();
        string[] answers = new string[3] { "hello", "world", "foo" };
        mock.When("sample-data/wordele-word-library.json").RespondJson(answers);
        var cut = ctx.RenderComponent<PlayWordele>(parameters => parameters.Add(p => p.answers, answers).Add(p => p.answer, "world"));

        cut.Find("input").Change("world");

        cut.Find("button").Click();

        // Assert
        cut.Find("strong").MarkupMatches("<strong>Attempt #: 1</strong>");
    }

    [Fact]
    public void SubmittingWrongGuessWithMultipleAnswers()
    {
        // Arrange
        using var ctx = new TestContext();

        // Act
        var mock = ctx.Services.AddMockHttpClient();
        string[] answers = new string[3] { "hello", "world", "foo" };
        mock.When("sample-data/wordele-word-library.json").RespondJson(answers);
        var cut = ctx.RenderComponent<PlayWordele>(parameters => parameters.Add(p => p.answers, answers).Add(p => p.answer, "world"));

        cut.Find("input").Change("hello");

        cut.Find("button").Click();

        // Assert
        cut.Find("strong").MarkupMatches("<strong>Attempt #: 2</strong>");
    }
}
