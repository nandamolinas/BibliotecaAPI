import React, { useState } from "react";

// Definindo o tipo para os dados que o usuário irá fornecer
interface DevolucaoRequest {
  IdLivro: number;
}

const DevolucaoEmprestimo = () => {
  const [idLivro, setIdLivro] = useState<number>(0); // Estado para armazenar o ID do livro a ser devolvido
  const [mensagem, setMensagem] = useState<string>(""); // Estado para mensagens de sucesso ou erro

  // Função para lidar com a devolução
  const devolverEmprestimo = () => {
    const payload: DevolucaoRequest = { IdLivro: idLivro };

    fetch("http://localhost:5200/api/emprestimos/devolver", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(payload),
    })
      .then((response) => {
        if (response.ok) {
          setMensagem("Empréstimo devolvido com sucesso!");
        } else {
          throw new Error("Erro ao devolver o empréstimo");
        }
      })
      .catch((error) => {
        setMensagem(error.message);
      });
  };

  return (
    <div>
      <h2>Devolver Empréstimo</h2>

      {/* Formulário para devolver um livro */}
      <div>
        <label htmlFor="idLivro">ID do Livro:</label>
        <input
          type="number"
          id="idLivro"
          value={idLivro}
          onChange={(e) => setIdLivro(Number(e.target.value))}
        />
      </div>

      <button onClick={devolverEmprestimo}>Devolver</button>

      {/* Exibição de mensagens */}
      {mensagem && <p>{mensagem}</p>}
    </div>
  );
};

export default DevolucaoEmprestimo;
