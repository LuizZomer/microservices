# Documento de Requisitos

## 1. Propósito do Sistema

O sistema permite o gerenciamento de produtos e estoques, incluindo:

- Operações de **cadastro**, **consulta**, **atualização** e **exclusão** de produtos.
- Notificações para **estoque crítico**.
- Garantia de eficiência e integração por meio de **APIs REST**.

---

## 2. Usuários

- **Administradores do Sistema**:  
  Realizam operações de cadastro e manutenção de produtos e estoques.

- **Equipe de Estoque**:  
  Monitora e gerencia o nível de produtos no inventário.

- **Vendedores**:  
  Consultam a disponibilidade de produtos.

---

## 3. Requisitos Funcionais

1. **Gerenciamento de Produtos**:  
   Permitir cadastro, consulta, atualização e exclusão de produtos.

2. **Gestão de Estoque**:  
   - Adicionar ou retirar produtos do estoque.
   - Monitorar níveis de estoque de forma automatizada.

3. **Notificações**:  
   Gerar alertas automáticos quando o estoque atingir níveis críticos.

4. **Integração de Serviços**:  
   Garantir consistência e validação de dados entre os seguintes serviços:  
   - **Serviço de Produtos**: Consulta de detalhes de produtos.  
   - **Serviço de Estoque**: Monitoramento de níveis críticos.  
   - **Serviço de Vendas**: Atualização de estoque após transações.

---

## 4. Fluxo das Integrações

- **Consulta de Produtos (Busca 1)**:  
  O serviço de Estoque consulta os detalhes de um produto no serviço de Produtos para validar informações.

- **Monitoramento de Estoque (Busca 2)**:  
  O serviço de Notificações verifica o estoque e gera alertas para níveis baixos.

- **Atualização de Estoque**:  
  Sempre que ocorre uma alteração no estoque, o sistema dispara uma notificação de alerta, se necessário.

---

# Descritivo Técnico

## Microsserviços

### 1. Serviço de Produtos  
**Função**: Gerencia informações dos produtos (nome, preço, descrição, etc.).  
**Endpoints**:
- **GET /products**: Lista todos os produtos.
- **GET /products/{id}**: Retorna os detalhes de um produto específico.
- **POST /products**: Cria um novo produto.
- **PUT /products/{id}**: Atualiza os dados de um produto.
- **DELETE /products/{id}**: Remove um produto.

---

### 2. Serviço de Estoque  
**Função**: Gerencia o inventário de produtos.  
**Endpoints**:
- **GET /stocks/{productId}**: Retorna a quantidade em estoque de um produto.
- **POST /stocks**: Adiciona ou inicializa o estoque de um produto.
- **PUT /stocks/{productId}**: Atualiza a quantidade disponível.

---

### 3. Serviço de Notificações  
**Função**: Envia alertas de estoque baixo.  
**Endpoints**:
- **POST /notifications**: Dispara notificações relacionadas ao estoque.

---

## Integrações

1. **Consulta de Produtos no Serviço de Estoque**:  
   O serviço de Estoque consulta o serviço de Produtos para validar informações ao registrar ou atualizar o inventário.

2. **Monitoramento de Estoque no Serviço de Notificações**:  
   O serviço de Notificações consulta o serviço de Estoque para monitorar produtos com estoque crítico.

3. **Atualizações Automáticas de Notificações**:  
   Atualizações no estoque disparam notificações automáticas para usuários cadastrados.
