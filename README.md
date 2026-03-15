# Lab.OptionsPattern

Projeto para apresentar o pattern options. Em resumo, o pattern consiste em carregar as configurações do appsettings.json e/ou
user secrets em classes tipadas, em vez de acessar valores soltos diretamente.

## Como funciona a configuração?

### Criando classe tipada

```c#
public class OpenAiSettings
{
    [Required]
    public string ApiKey { get; set; }

    [Required]
    public string BaseAddress { get; set; }
}
```

O exemplo acima reflete as propriedade criadas dentro do `appsettings.json`:
```
"Providers": {
    "OpenAI": {
      "BaseAddress": "http://teste.api.com.br",
      "ApiKey": ""
    }
  }
```

## Realizando o bind

```c#
builder.Services.AddOptionsWithValidateOnStart<OpenAiSettings>()
    .Bind(builder.Configuration.GetSection("Providers:OpenAI"))
    .ValidateDataAnnotations();
```

O bind das propriedades do `appsettings.json` com o objeto da classe ocorre através da configuração acima, onde é obtido a seção
e aplicado a validação dos data annotations.

## Utilizando as propriedades

```
internal class OpenAIGateway(IOptions<OpenAiSettings> openAiSettings)
{
    private readonly string _baseAddress = openAiSettings.Value.BaseAddress;
    private readonly string _apiKey = openAiSettings.Value.ApiKey;

    public Task<string> ExecutePrompt(string prompt)
    {
        return Task.FromResult("Resultado "+ _baseAddress);
    }
}
```

Através da injeção `IOptions<OpenAiSettings> openAiSettings` é possível acessar as propriedades com os valores carregados
do `appsettings.json` da seguinte forma `openAiSettings.Value.ApiKey` ou `openAiSettings.Value.BaseAddress`.