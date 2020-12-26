import React, { Component } from "react";
import { TodoItem } from "./TodoItem";

const API_URL_TODO = "api/mytodos";

export class TodoList extends Component {
    static displayName = TodoList.name;

    constructor(props) {
        super(props);
        this.state = {
            todoList: { owner: "", todos: [] },
            loading: true,
            newTask: "",
        };
        this.toggleCompletion = this.toggleCompletion.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    componentDidMount() {
        this.populateData();
    }

    toggleCompletion(id) {
        const newTodos = [...this.state.todoList.todos];
        const todo = newTodos.find((x) => x.id === id);
        todo.completed = !todo.completed;
        this.update(todo).then((todo) => {
            this.setState(todo);
        });
    }

    handleSubmit(event) {
        this.create({ task: this.state.newTask }).then((todo) => {
            const todos = [...this.state.todoList.todos, todo];
            this.setState(todos);
        });
        event.preventDefault();
    }

    handleChange(event) {
        this.setState({ newTask: event.target.value });
    }

    renderTodosTable(todos) {
        return (
            <ul className="list-group list-group-flush">
                {todos.map((todo) => (
                    <li className="list-group-item" key={todo.id}>
                        <TodoItem
                            todo={todo}
                            onCompletionChange={this.toggleCompletion}
                        />
                    </li>
                ))}
            </ul>
        );
    }

    render() {
        let contents = this.state.loading ? (
            <p>
                <em>Loading...</em>
            </p>
        ) : (
            this.renderTodosTable(this.state.todoList.todos)
        );

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
                {contents}
            </div>
        );
    }

    async populateData() {
        this.setState({ loading: true });
        const response = await fetch(API_URL_TODO);
        const data = await response.json();
        this.setState({ todoList: data, loading: false });
    }

    async create(todo) {
        const response = await fetch(API_URL_TODO, {
            body: JSON.stringify(todo),
            headers: {
                "content-type": "application/json",
            },
            method: "POST",
        });
        return response.json();
    }

    async update(todo) {
        const response = await fetch(API_URL_TODO, {
            body: JSON.stringify(todo),
            headers: {
                "content-type": "application/json",
            },
            method: "PUT",
        });
        return response.json();
    }
}
