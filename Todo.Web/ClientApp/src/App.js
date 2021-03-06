import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/layouts/Layout';
import { Home } from './components/Home';
import { TodoList } from "./components/todo/TodoList";

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <Layout>
        <Route exact path="/" component={Home} />
        <Route path="/todos" component={TodoList} />
      </Layout>
    );
  }
}
