using ProvaTecnica.Dominio.Entidades;
using ProvaTecnica.Dominio.Models;
var builder = WebApplication.CreateBuilder(args);


// Adiciona o Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();


// Configura o Swagger e SwaggerUI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



#region Variaveis Globais

//Variaveis que serão usadas em todo codigo

var caminhoJsonPessoas = Path.Combine("Dominio", "pessoas.json");

var pessoa = new Pessoa();

#endregion


#region Endpoints - Pessoa

app.MapGet("/pessoa/listar", async () =>
{
    try
    {
        var listaPessoas = await pessoa.DesserializarJsonAsync(caminhoJsonPessoas);
        return Results.Ok(listaPessoas);
    }
    catch (Exception ex)
    {
        return Results.NotFound($"Erro ao listar pessoas: {ex.Message}");
    }

});

app.MapGet("pessoa/obterPorEmail/{email}", async (string email) =>
{
    try
    {
        var pessoaEncontrada = await pessoa.ObterPessoaPorEmailAsync(email, caminhoJsonPessoas);

        return Results.Ok(pessoaEncontrada);

    }
    catch (Exception ex)
    {
        return Results.NotFound($"Erro ao buscar pessoa pelo email: {ex.Message}");
    }
});

app.MapGet("pessoa/obterPorId/{id}", async (int id) =>
{
    try
    {
        var pessoaEncontrada = await pessoa.ObterPessoaPorIdAsync(id, caminhoJsonPessoas);

        return Results.Ok(pessoaEncontrada);

    }
    catch (Exception ex)
    {
        return Results.NotFound($"Erro ao buscar pessoa pelo id: {ex.Message}");
    }
});

app.MapPut("pessoa/atualizar/{id}", async (int id, PessoaDTO pessoaAtualizar) =>
{
    try
    {
        await pessoa.AtualizarPessoaAsync(pessoaAtualizar, id, caminhoJsonPessoas);
        return Results.Ok("Pessoa atualizada com sucesso");

    }
    catch (Exception ex)
    {
        return Results.NotFound($"Erro ao atualizar pessoa: {ex.Message}");
    }
});

app.MapPost("pessoa/adicionar", async (PessoaDTO pessoaAdicionar) =>{
    try
    {
        await pessoa.AdicionarPessoaAsync(pessoaAdicionar, caminhoJsonPessoas);
        return Results.Ok("Pessoa adicionada com sucesso");
    }
    catch (Exception ex)
    {
        return Results.NotFound($"Erro ao adicionar pessoa: {ex.Message}");
    }
});

app.MapDelete("pessoa/excluir/{id}", async (int id) =>{
    try
    {
        await pessoa.RemoverPessoaPorIdAsync(id, caminhoJsonPessoas);
        return Results.Ok("Pessoa excluída com sucesso");
    }
    catch (Exception ex)
    {
        return Results.NotFound($"Erro ao excluir pessoa: {ex.Message}");
    }
});

#endregion



app.Run();
