<template>
<section>
    <h1 class="page-title">{{$t('passwordreset.title')}}</h1>
    <div class="main">
        <div v-if="$route.params.token">
            <div class="form-field">
                <label for="passwordInput">{{$t('menus.password')}}</label>
                <input id="passwordInput" @keyup.enter="resetPassword" v-model="newPassword" required />
            </div>
            <button @click="resetPassword" class="button-primary">{{$t('menus.reset')}}</button>
        </div>
        <div v-else>
            <div class="form-field">
                <label for="emailInput">{{$t('passwordreset.enterMail')}}</label>
                <input id="emailInput" @keyup.enter="initPasswordReset" type="email" v-model="email" required />
            </div>
            <button class="button-primary" @click="initPasswordReset">{{$t('passwordreset.initiateReset')}}</button>
        </div>
    </div>
</section>
</template>

<style scoped>
    .form-field input {
        max-width: 380px;
    }
</style>


<script>
import { mapActions } from "vuex";
export default {
    name: "PasswordReset",
    data() {
        return {
            email: "",
            newPassword: "",
        }
    },
    methods: {
        ...mapActions("context", ["passwordResetFinish", "passwordResetInit", "login"]),
        resetPassword()
        {
            if (!this.newPassword)
            {
                this.$notifyError('Zadaj heslo');
                return;
            }
            this.passwordResetFinish({ 
                password: this.newPassword, 
                userId: this.$route.params.userId, 
                token: this.$route.params.token})
                .then(() => {
                    this.$notifySuccess("Zmenené");
                    this.$router.push({ name: 'home' });
                })
                .catch((err, p) =>{ 
                    console.log('aaaaaaaaaaaa koniec', err, p);
                    console.log(err); 
                    this.$notifyError(err?.response?.data); });
        },
        initPasswordReset() 
        {
            if (!this.email)
            {
                this.$notifyError('Zadaj email');
                return;
            }
            this.passwordResetInit(this.email)
                .then(() => this.$notifySuccess('Pozri mail'))
                .catch((err) => this.$notifyError(err));
        },
    },
    mounted() {
    }
}
</script>
