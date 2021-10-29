package com.youzack.LoginApp.ui.login;

import android.app.Activity;

import androidx.lifecycle.Observer;
import androidx.lifecycle.ViewModelProvider;

import android.os.Bundle;

import androidx.annotation.Nullable;
import androidx.annotation.StringRes;
import androidx.appcompat.app.AppCompatActivity;

import android.os.Handler;
import android.text.Editable;
import android.text.TextWatcher;
import android.view.KeyEvent;
import android.view.View;
import android.view.inputmethod.EditorInfo;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;

import com.youzack.LoginApp.R;
import com.youzack.LoginApp.ui.login.LoginViewModel;
import com.youzack.LoginApp.ui.login.LoginViewModelFactory;
import com.youzack.LoginApp.databinding.ActivityLoginBinding;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;

import okhttp3.Call;
import okhttp3.MediaType;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.RequestBody;

public class LoginActivity extends AppCompatActivity {

    private LoginViewModel loginViewModel;
    private ActivityLoginBinding binding;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        binding = ActivityLoginBinding.inflate(getLayoutInflater());
        setContentView(binding.getRoot());

        loginViewModel = new ViewModelProvider(this, new LoginViewModelFactory())
                .get(LoginViewModel.class);

        final EditText usernameEditText = binding.username;
        final EditText passwordEditText = binding.password;
        final Button loginButton = binding.login;
        final ProgressBar loadingProgressBar = binding.loading;

        loginButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                loadingProgressBar.setVisibility(View.VISIBLE);
                String username=usernameEditText.getText().toString();
                String password =  passwordEditText.getText().toString();
                try {
                    login(username,password);
                } catch (Exception e) {
                    e.printStackTrace();
                }
            }
        });
    }

    private void login(String username,String password) {
        String httpUrl  = "https://192.168.0.106:44360/api/Login/Login";
        Handler handler = new Handler();
        new Thread(()->{
            OkHttpClient.Builder builder = new OkHttpClient.Builder();
            OkHttpClient httpClient= builder
                    .sslSocketFactory(OkHttpUtils.createSSLSocketFactory(),
                            new OkHttpUtils.TrustAllManager())
                    .hostnameVerifier(new OkHttpUtils.TrustAllHostnameVerifier())
                    .retryOnConnectionFailure(true).build();

            Request.Builder requestBuilder =new Request.Builder()
                    .url(httpUrl).post(RequestBody.create(MediaType.parse("application/json"),
                            "{'username':'"+username+"','password':'"+password+"'}"));
            Request request = requestBuilder.build();
            Call call = httpClient.newCall(request);
            String json = null;
            try {
                json = call.execute().body().string();
            } catch (IOException e) {
                throw new RuntimeException(e);
            }
            System.out.println(json);
        }).start();
        handler.post(()->{

        });
    }
}