<template>
    <header>
        <nav class="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">
            <div class="container">
                <a class="navbar-brand"><img src="../assets/logo.png" width="50" height="40" />Book club!</a>
                <button class="navbar-toggler"
                        type="button"
                        data-toggle="collapse"
                        data-target=".navbar-collapse"
                        aria-label="Toggle navigation"
                        @click="toggle">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="navbar-collapse collapse d-sm-inline-flex flex-sm-row-reverse"
                     v-bind:class="{show: isExpanded}">
                    <ul class="navbar-nav flex-grow">
                        <li class="nav-item">
                            <router-link :to="{ name: 'home' }" class="nav-link text-dark">🏠 Domov</router-link>
                        </li>
                        <li class="nav-item" v-if="isMod">
                            <router-link :to="{ name: 'admin' }" class="nav-link text-dark">🧛🏻‍♀️ Admin</router-link>
                        </li>
                        <li class="nav-item" v-if="isAuthenticated">
                            <router-link :to="{ name: 'booklist' }" class="nav-link text-dark">📚 Knihy</router-link>
                        </li>
                        <li class="nav-item" v-if="isAuthenticated">
                            <router-link :to="{ name: 'roundlist' }" class="nav-link text-dark">💬 Návrhy</router-link>
                        </li>
                        <li class="nav-item" v-if="isAuthenticated">
                            <router-link :to="{ name: 'recommendationlist' }" class="nav-link text-dark">👌🏻 Odporúčania</router-link>
                        </li>
                        <li class="nav-item" v-if="isAuthenticated">
                            <router-link :to="{ name: 'userlist' }" class="nav-link text-dark">🤼 My</router-link>
                        </li>
                        <li><login-control></login-control></li>
                    </ul>
                </div>
            </div>
        </nav>
    </header>
</template>


<style>
    a.navbar-brand {
        white-space: normal;
        text-align: center;
        word-break: break-all;
    }

    html {
        font-size: 14px;
    }

    @media (min-width: 768px) {
        html {
            font-size: 16px;
        }
    }

    .box-shadow {
        box-shadow: 0 .25rem .75rem rgba(0, 0, 0, .05);
    }
</style>

<script>
import { mapGetters } from "vuex";
import LoginControl from "./LoginControl.vue";
export default {
    name: "NavMenu",
    components: { LoginControl },
    data() {
        return {
            isExpanded: false
        }
    },
    computed: {
        ...mapGetters("context", ["isAuthenticated", "isMod"])
    },
    methods: {
        collapse() {
            this.isExpanded = false;
        },

        toggle() {
            this.isExpanded = !this.isExpanded;
        }
    }
}
</script>