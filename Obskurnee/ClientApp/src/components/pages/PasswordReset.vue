<template>
<section>
 
</section>
</template>


<script>
import axios from 'axios';
import { mapActions, mapState, mapGetters } from "vuex";
export default {
    name: "PasswordReset",
    data() {
        return {
        }
    },
    methods: {
        ...mapActions("context", ["passwordResetFinish"]),
    },
    mounted() {
    }
}
</script>

<style scoped>
    .main {
        padding: var(--spacer);
        background-color: var(--c-bckgr-primary);
        max-width: 800px;
        margin: 0 var(--spacer) calc(var(--spacer) * 2) var(--spacer);
    }

    @media screen and (min-width: 840px) {
        .main {
            margin: 0 auto var(--spacer) auto;
        }
    }

    .not-mod-text {
        font-size: 1.5em;
        font-weight: bold;
        text-align: center;
    }

    .not-mod-pic {
        max-width: 400px;
        margin: 0 auto;
    }

    .not-mod img {
        width: 100%;
    }

    .section {
        margin-bottom: calc(2* var(--spacer));
    }

    .section-title {
        margin-top: 0;
        margin-bottom: 0.5em;
    }

    .form-field label {
        display: block;
        margin-bottom: calc(var(--spacer) / 2);
    }

    .form-field textarea {
        width: 100%;
        height: 15em;
        margin-bottom: calc(var(--spacer) / 2);
    }

    .form-field input {
        width: 100%;
        max-width: 380px;
        margin-bottom: calc(var(--spacer) / 2);
    }

    .note {
        display: flex;
        align-items: center;
        margin-top: var(--spacer);
        padding: 0 calc(var(--spacer) / 2);
    }

    .note-pic {
        width: 20px;
        flex-shrink: 0;
    }

    .note-pic img {
        width: 100%;
    }

    .note-text {
        font-size: 0.875em;
        opacity: 0.8;
        margin: 0 0 0 calc(var(--spacer) / 2);
    }

</style>