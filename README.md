# UnicornOne Workbook Command Test

A minimal UnicornOne workbook plugin for testing execution of Recipe API commands from the Workbook API.

The plugin provides a simple command entry box and **Send!** button, allowing recipe commands to be issued directly from a workbook without running an IronPython recipe.

## Purpose

This project is primarily a development and integration test for the command path:

    Workbook Plugin
        ↓
    IWorkbookHost.ExecutePythonCommand()
        ↓
    GenericWorkbookHostForm
        ↓
    RecipeApi
        ↓
    UnicornOne

It demonstrates that workbook plugins can access the existing UnicornOne Recipe API without directly referencing or reimplementing recipe functionality.

## Usage

Enter a Recipe API command into the command box and click **Send!**.

For example:

    WriteMessage(Hello from workbook!)

    Open(Ga1)

    Close(Ga1)

    Ramp(Manipulator.Heater, 700, 2)

    Set(Manipulator.Heater.SP, 700)

Multiple arguments are comma-separated.

The current command parser is intentionally simple and assumes that individual arguments do not themselves contain commas.

## How It Works

The workbook calls:

    IWorkbookHost.ExecutePythonCommand(string command)

The UnicornOne workbook host parses the command into:

- Command name
- Argument list

The command name is matched against the available `RecipeApi` methods using reflection.

Arguments are converted to the parameter types required by the matching method, and the method is invoked on the workbook host's `RecipeApi` instance.

This allows the Recipe API itself to remain the source of truth for available commands rather than maintaining a separate command switch or command registry.

## RecipeApi

`RecipeApi` is the same API exposed to UnicornOne IronPython recipes.

For normal Python recipe execution it is instantiated with an `IRecipeHost` and the associated cancellation-token functions.

For lightweight consumers such as workbook plugins, `RecipeApi` can operate with its default/null host implementation, allowing commands to be executed without creating an IronPython runtime.

## Plugin Structure

The plugin deliberately contains very little functionality:

- Text box for entering a command
- **Send!** button
- Call to `IWorkbookHost.ExecutePythonCommand()`
- Basic logging/error handling

It does not directly access UnicornOne hardware, configuration, recipe internals, or `RecipeApi`.

This separation is intentional: workbook plugins communicate through the public Workbook API, while UnicornOne remains responsible for command execution.

## Building

Build the project using:

    dotnet build

The resulting DLL can then be deployed to the UnicornOne workbook plugin directory:

    Plugins/Workbooks/

The workbook host dynamically discovers the `IWorkbookPlugin` implementation from the assembly.

## Status

Development / test plugin.

This project exists primarily to validate the Workbook → RecipeApi command bridge and provide a simple environment for testing new workbook-accessible recipe commands.

## License

Part of the UnicornOne development ecosystem.
