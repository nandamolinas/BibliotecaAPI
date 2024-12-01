// src/Componentes/CadastrarEmprestimo.tsx

import React, { useState, useEffect } from "react";
import axios from "axios";
import { Emprestimo } from "../../models/Emprestimo";


const CadastrarEmprestimo = () => {
  const [livros, setLivros] = useState<any[]>([]); // Lista de livros disponíveis para empréstimo
  const [clientes, setClientes] = useState<any[]>([]); // Lista de clientes
  const [livroId, setLivroId] = useState<number | undefined>(undefined);
  const [clienteId, setClienteId] = useState<number | undefined>(undefined);
  const [dataDeEmprestimo, setDataDeEmprestimo] = useState<string>("");
  const [dataDeDevolucao, setDataDeDevolucao] = useState<string | null>("");

  useEffect(() => {
    // Carregar livros e clientes
    axios.get("http://localhost:5200/api/livros/listar").then((response) => {
      setLivros(response.data);
    });
    axios.get("http://localhost:5200/api/clientes/listar").then((response) => {
      setClientes(response.data);
    });
  }, []);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();

    const novoEmprestimo: Emprestimo = {
      id: 0, // Novo empréstimo, o ID será gerado pelo banco
      livroId: livroId!,
      clienteId: clienteId!,
      dataDeEmprestimo,
      dataDeDevolucao,
    };

    axios
      .post("http://localhost:5200/api/emprestimos/cadastrar", novoEmprestimo)
      .then((response) => {
        console.log("Empréstimo cadastrado com sucesso:", response.data);
      })
      .catch((error) => {
        console.error("Erro ao cadastrar empréstimo:", error);
      });
  };

  return (
    <div>
      <h1>Cadastrar Empréstimo</h1>
      <form onSubmit={handleSubmit}>
        <div>
          <label htmlFor="livroId">Livro</label>
          <select
            id="livroId"
            value={livroId}
            onChange={(e) => setLivroId(Number(e.target.value))}
            required
          >
            <option value="">Selecione um livro</option>
            {livros.map((livro: any) => (
              <option key={livro.id} value={livro.id}>
                {livro.titulo}
              </option>
            ))}
          </select>
        </div>

        <div>
          <label htmlFor="clienteId">Cliente</label>
          <select
            id="clienteId"
            value={clienteId}
            onChange={(e) => setClienteId(Number(e.target.value))}
            required
          >
            <option value="">Selecione um cliente</option>
            {clientes.map((cliente: any) => (
              <option key={cliente.id} value={cliente.id}>
                {cliente.nome}
              </option>
            ))}
          </select>
        </div>

        <div>
          <label htmlFor="dataDeEmprestimo">Data de Empréstimo</label>
          <input
            type="date"
            id="dataDeEmprestimo"
            value={dataDeEmprestimo}
            onChange={(e) => setDataDeEmprestimo(e.target.value)}
            required
          />
        </div>

        <div>
          <label htmlFor="dataDeDevolucao">Data de Devolução</label>
          <input
            type="date"
            id="dataDeDevolucao"
            value={dataDeDevolucao || ""}
            onChange={(e) => setDataDeDevolucao(e.target.value || null)}
          />
        </div>

        <div>
          <button type="submit">Cadastrar Empréstimo</button>
        </div>
      </form>
    </div>
  );
};

export default CadastrarEmprestimo;
