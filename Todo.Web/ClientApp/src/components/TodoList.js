import React, { Component } from "react";

export class TodoList extends Component {
    static displayName = TodoList.name;

    constructor(props) {
        super(props);
        this.state = { todoList: { owner: "", todos: [] }, loading: true };
    }

    componentDidMount() {
        this.populateData();
    }

    static formatDate(date) {
        if (date) {
            const newDate = new Date(date);
            return newDate.toLocaleString();
        } else {
            return "N/A";
        }
    }

    static renderTodosTable(todos) {
        return (
            <table className="table table-striped" aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th></th>
                        <th>Task</th>
                        <th>Deadline</th>
                    </tr>
                </thead>
                <tbody>
                    {todos.map((todo) => (
                        <tr key={todo.id}>
                            <td>
                                <input type="checkbox" checked={todo.isDone} />
                            </td>
                            <td>{todo.task}</td>
                            <td>{TodoList.formatDate(todo.deadline)}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading ? (
            <p>
                <em>Loading...</em>
            </p>
        ) : (
            TodoList.renderTodosTable(this.state.todoList.todos)
        );

        return (
            <div>
                <h1 id="tabelLabel">Todo List</h1>
                <p>
                    This component demonstrates fetching data from the server.
                </p>
                {contents}
            </div>
        );
    }

    async populateData() {
        const response = await fetch("api/mytodos");
        const data = await response.json();

        console.log(JSON.stringify(data, null, 2));

        this.setState({ todoList: data, loading: false });
    }
}
