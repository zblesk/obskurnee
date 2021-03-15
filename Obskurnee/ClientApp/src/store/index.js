import { createStore } from 'vuex'
import context from './context'
import rounds from './rounds'
import discussions from './discussions'
import polls from './polls'
import users from './users'
import books from './books'
import recommendations from './recommendations'
import global from './global'

export default createStore({
  modules: {
    context,
    rounds,
    discussions,
    polls,
    users,
    books,
    recommendations,
    global,
  }
})