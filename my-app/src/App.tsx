// App.tsx
import React from "react";
import { BrowserRouter, Link, Route, Routes } from "react-router-dom";

import CadastrarLivros from "./Componentes/Listagem/CadastrarLivros";
import ListarEmprestimos from "./Componentes/Listagem/ListarEmprestimos";
import ListarLivros from "./Componentes/Listagem/LIstarLIvro";
import CadastrarEmprestimo from "./Componentes/Listagem/CadastrarEmprestimo";
import CadastrarCliente from "./Componentes/Listagem/CadastrarCliente";
import ListarClientes from "./Componentes/Listagem/ListarCientes";
import ExcluirCliente from "./Componentes/Listagem/ExclusãoCliente";
import DevolucaoEmprestimo from "./Componentes/Listagem/DevoluçãoEmprestimos";

function App() {
  return (
    <div>
      <div id="app">
        <BrowserRouter>
          <nav>
            <ul>
              <li>
                <Link to="/">Home</Link>
              </li>
              <li>
                <Link to="/livros/listar">Listar Livros</Link>
              </li>
              <li>
                <Link to="/livros/cadastrar">Cadastrar Livros</Link>
              </li>
              <li>
                <Link to="/emprestimos/listar">Listar Empréstimos</Link>
              </li>
              <li>
                <Link to="/emprestimos/cadastrar">Cadastrar Empréstimos</Link>
              </li>
              <li>
                <Link to="/clientes/listar">Listar Clientes</Link>
              </li>
              <li>
                <Link to="/clientes/cadastrar">Cadastrar Cliente</Link>
              </li>
              <li>
                <Link to="/clientes/excluir">Excluir Cliente</Link> {/* Link para excluir cliente */}
              </li>
              <li>
                <Link to="/emprestimos/devolver">Devolver Empréstimo</Link>
              </li>
            </ul>
          </nav>

          <div id="conteudo">
            <Routes>
              <Route path="/" element={<ListarLivros />} />
              <Route path="/livros/listar" element={<ListarLivros />} />
              <Route path="/livros/cadastrar" element={<CadastrarLivros />} />
              <Route path="/emprestimos/listar" element={<ListarEmprestimos />} />
              <Route path="/emprestimos/cadastrar" element={<CadastrarEmprestimo />} />
              <Route path="/clientes/listar" element={<ListarClientes />} />
              <Route path="/clientes/cadastrar" element={<CadastrarCliente />} />
              <Route path="/clientes/excluir" element={<ExcluirCliente />} /> {/* Rota para excluir cliente */}
              <Route path="/emprestimos/devolver" element={<DevolucaoEmprestimo />} />
            </Routes>
          </div>
        </BrowserRouter>
      </div>
    </div>
  );
}

export default App;
