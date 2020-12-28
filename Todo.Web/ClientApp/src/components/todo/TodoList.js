import React, { Component } from "react";
import { UncontrolledAlert, Spinner } from "reactstrap";

import authService from "../api-authorization/AuthorizeService";
import { TodoItem } from "./TodoItem";

const API_URL_TODO = "api/mytodos";

export class TodoList extends Component {
  static displayName = TodoList.name;

  constructor(props) {
    super(props);
    this.state = {
      todos: [],
      loading: true,
      newTask: "",
      error: null,
    };
    this.toggleCompletion = this.toggleCompletion.bind(this);
    this.handleChange = this.handleChange.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
  }

  componentDidMount() {
    this.populateData();
  }

  toggleCompletion(id) {
    const newTodos = [...this.state.todos];
    const todo = newTodos.find((x) => x.id === id);
    todo.completed = !todo.completed;
    this.update(todo);
  }

  handleSubmit(event) {
    event.preventDefault();
    const task = this.state.newTask;
    if (task === "") return;
    const todo = { task: task };
    this.create(todo);
  }

  handleChange(event) {
    this.setState({ newTask: event.target.value });
  }

  renderTodosTable(todos) {
    return (
      <ul className="list-group list-group-flush">
        {todos.map((todo) => (
          <li className="list-group-item" key={todo.id}>
            <TodoItem todo={todo} onCompletionChange={this.toggleCompletion} />
          </li>
        ))}
      </ul>
    );
  }

  render() {
    let contents = this.state.loading ? (
      <Spinner type="grow" color="danger" />
    ) : (
      this.renderTodosTable(this.state.todos)
    );

    let notifications = this.state.error ? (
      <UncontrolledAlert color="danger">{this.state.error}</UncontrolledAlert>
    ) : null;

    return (
      <div>
        <h1>Things do do</h1>
        <form className="row g-3">
          <div className="col-auto">
            <input
              type="text"
              className="form-control"
              value={this.state.newTask}
              onChange={this.handleChange}
            />
          </div>
          <div className="col-auto">
            <button
              type="submit"
              className="btn btn-primary mb-3"
              onClick={this.handleSubmit}
            >
              ADD
            </button>
          </div>
        </form>
        {notifications}
        {contents}
      </div>
    );
  }

  async populateData() {
    this.setState({ loading: true, error: null });
    const token = await authService.getAccessToken();
    const response = await fetch(API_URL_TODO, {
      headers: !token ? {} : { Authorization: `Bearer ${token}` },
    });

    if (response.ok) {
      const data = await response.json();
      this.setState({ todos: data, loading: false });
    } else {
      this.setState({
        error: `error status: ${response.status} ${response.statusText}`,
      });
    }
  }

  async create(todo) {
    this.setState({ loading: true, error: null });
    const token = await authService.getAccessToken();
    const response = await fetch(API_URL_TODO, {
      body: JSON.stringify(todo),
      headers: !token
        ? { "Content-Type": "application/json" }
        : {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
          },
      method: "POST",
    });

    if (response.ok) {
      const todo = await response.json();
      const todos = [...this.state.todos, todo];
      this.setState({ todos: todos });
    } else {
      this.setState({
        error: `error status: ${response.status} ${response.statusText}`,
      });
    }
    this.setState({ loading: false });
  }

  async update(todo) {
    this.setState({ loading: true, error: null });
    const token = await authService.getAccessToken();
    const response = await fetch(API_URL_TODO, {
      body: JSON.stringify(todo),
      headers: !token
        ? { "Content-Type": "application/json" }
        : {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
          },
      method: "PUT",
    });

    if (response.ok) {
      const todo = await response.json();
      this.setState(todo);
    } else {
      this.setState({
        error: `error status: ${response.status} ${response.statusText}`,
      });
    }
    this.setState({ loading: false });
  }
}
