# 🚀 Guia Prático: Criptografando o CPF com Hash no ASP.NET Core MVC e CUSTOMIZAÇÃO DE LAYOUT

## 📌 PARTE 01 - HASH


🎯 **Objetivo**
Transformar o CPF digitado pelo usuário em um Hash SHA-256 de 64 caracteres antes de persistir no banco de dados, tanto na criação quanto na edição do registro.

**Passo 1:** Adicionar os using necessários
No início do arquivo ClienteController.cs, adicione os namespaces de criptografia do C#:

```csharp
using System.Text;
using System.Security.Cryptography;

```

**Passo 2:** Criar o Método Auxiliar de Hash
No final da classe ClienteController (próximo ao método ClienteExists), adicione o método privado GerarHashCpf:

```csharp
private string GerarHashCpf(string cpf)
{
    if (string.IsNullOrWhiteSpace(cpf))
        return string.Empty;

    // 1. Remove caracteres não numéricos (pontos, traços, espaços)
    string cpfLimpo = string.Concat(Array.FindAll(cpf.ToCharArray(), char.IsDigit));

    // 2. Converte para bytes UTF-8
    byte[] inputBytes = Encoding.UTF8.GetBytes(cpfLimpo);

    // 3. Gera o Hash SHA-256
    byte[] hashBytes = SHA256.HashData(inputBytes);

    // 4. Retorna em formato Hexadecimal (64 caracteres)            
    return Convert.ToHexString(hashBytes).ToLowerInvariant();
}

```

***Passo 3:** Atualizar a ação Create (POST)
No método Create, chame o GerarHashCpf antes de salvar o objeto com _context.Add(cliente):

```csharp
// POST: Cliente/Create
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create([Bind("Codigo,Nome,Cpf,Telefone,DataNascimento")] Cliente cliente)
{
    if (ModelState.IsValid)
    {
        // 🔒 Aplica o Hash no CPF antes de salvar no banco
        cliente.Cpf = GerarHashCpf(cliente.Cpf);

        _context.Add(cliente);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    return View(cliente);
}


```


**Passo 4:** Atualizar a ação Edit (POST)
No método Edit, aplique o mesmo tratamento antes do _context.Update(cliente) para garantir que novas alterações no CPF também sejam convertidas em hash:

```csharp
// POST: Cliente/Edit/5
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(int id, [Bind("Codigo,Nome,Cpf,Telefone,DataNascimento")] Cliente cliente)
{
    if (id != cliente.Codigo)
    {
        return NotFound();
    }

    if (ModelState.IsValid)
    {
        try
        {
            // 🔒 Garante que o novo CPF editado também seja convertido em Hash
            cliente.Cpf = GerarHashCpf(cliente.Cpf);

            _context.Update(cliente);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ClienteExists(cliente.Codigo))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
        return RedirectToAction(nameof(Index));
    }
    return View(cliente);
}

```



---

## 🛠️ TAREFAS ORGANIZAÇÃO LAYOUT E SEGURANÇA

***SEGURANÇA***

Crie uma tabela USUARIO em seu banco de dados com os campos Codigo, username, senha.

Você deve alterar o seu sistema/login para o usuário informar o nome e senha.
O usuário só deve acessar os módulos do sistema se tiver logado.

Por segurança na tela de login não deve conter nenhum menu de acesso direto.

SUGESTÃO
Na pasta shared, crie um Layout especifico para Login.

